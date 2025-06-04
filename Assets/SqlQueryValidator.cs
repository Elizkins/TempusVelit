using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using TempusVelit.Database;

namespace TempusVelit.Assets
{
    public class SqlQueryValidator
    {
        private readonly TSqlParser _parser;

        public SqlQueryValidator()
        {
            _parser = new TSql150Parser(true);
        }

        public ValidationResult ValidateQuery(string userQuery, ControlTask task)
        {
            var analysisResult = AnalyzeQuery(userQuery);

            if (!analysisResult.IsValid)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Errors = analysisResult.Errors,
                    ErrorType = ValidationErrorType.SyntaxError
                };
            }

            var validationResult = new ValidationResult();

            var missingTables = task.RequiredTables.Split(',')
                                                    .Select(s => s.Trim().Trim('"'))
                                                    .ToArray()
                                                    .Except(analysisResult.TablesUsed, StringComparer.OrdinalIgnoreCase)
                                                    .ToList();

            var missingConditions = new List<string>();
            foreach (var requiredCondition in task.RequiredConditions.Split(',')
                                                                     .Select(s => s.Trim().Trim('"'))
                                                                     .ToArray())
            {
                if (!analysisResult.Conditions.Any(c =>
                    c.IndexOf(requiredCondition, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    missingConditions.Add(requiredCondition);
                }
            }

            var missingColumns = task.RequiredColumns.Split(',')
                                                     .Select(s => s.Trim().Trim('"'))
                                                     .ToArray()
                                                     .Except(analysisResult.SelectedColumns, StringComparer.OrdinalIgnoreCase)
                                                     .ToList();

            validationResult.IsValid = missingTables.Count == 0 &&
                                    missingConditions.Count == 0 &&
                                    missingColumns.Count == 0;

            validationResult.MissingTables = missingTables;
            validationResult.MissingConditions = missingConditions;
            validationResult.MissingColumns = missingColumns;

            if (!validationResult.IsValid)
            {
                validationResult.ErrorType = missingTables.Count > 0 ?
                    ValidationErrorType.MissingTables :
                    missingConditions.Count > 0 ?
                        ValidationErrorType.MissingConditions :
                        ValidationErrorType.MissingColumns;
            }

            return validationResult;
        }

        private SqlAnalysisResult AnalyzeQuery(string sqlQuery)
        {
            IList<ParseError> errors;
            var fragment = _parser.Parse(new StringReader(sqlQuery), out errors);

            if (errors.Count > 0)
            {
                return new SqlAnalysisResult
                {
                    IsValid = false,
                    Errors = errors.Select(e => e.Message).ToList()
                };
            }

            var visitor = new SqlAnalysisVisitor();
            fragment.Accept(visitor);

            return visitor.GetResult();
        }
    }

    public class SqlAnalysisResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> TablesUsed { get; set; } = new List<string>();
        public List<string> Conditions { get; set; } = new List<string>();
        public List<string> SelectedColumns { get; set; } = new List<string>();
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> MissingTables { get; set; } = new List<string>();
        public List<string> MissingConditions { get; set; } = new List<string>();
        public List<string> MissingColumns { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public ValidationErrorType ErrorType { get; set; }
    }

    public enum ValidationErrorType
    {
        None,
        SyntaxError,
        MissingTables,
        MissingConditions,
        MissingColumns
    }

    public class SqlAnalysisVisitor : TSqlFragmentVisitor
    {
        public List<string> TablesUsed { get; } = new List<string>();
        public List<string> Conditions { get; } = new List<string>();
        public List<string> SelectedColumns { get; } = new List<string>();
        private readonly SqlScriptGenerator _scriptGenerator = new Sql150ScriptGenerator();

        public override void Visit(NamedTableReference node)
        {
            TablesUsed.Add(node.SchemaObject.BaseIdentifier.Value);
            base.Visit(node);
        }

        public override void Visit(BooleanComparisonExpression node)
        {
            Conditions.Add(NormalizeCondition(node));
            base.Visit(node);
        }

        public override void Visit(SelectScalarExpression node)
        {
            if (node.ColumnName != null)
            {
                SelectedColumns.Add(node.ColumnName.Value);
            }
            else if (node.Expression is ColumnReferenceExpression columnRef)
            {
                SelectedColumns.Add(GetColumnName(columnRef));
            }
            base.Visit(node);
        }

        private string NormalizeCondition(BooleanComparisonExpression node)
        {
            var left = GetExpressionText(node.FirstExpression);
            var right = GetExpressionText(node.SecondExpression);
            return $"{left} {GetOperatorText(node.ComparisonType)} {right}";
        }

        private string GetExpressionText(ScalarExpression expression)
        {
            switch (expression)
            {
                case ColumnReferenceExpression colRef:
                    return GetColumnName(colRef);
                case Literal literal:
                    return literal.Value;
                default:
                    return GenerateScript(expression);
            }
        }

        private string GetColumnName(ColumnReferenceExpression columnRef)
        {
            return string.Join(".", columnRef.MultiPartIdentifier.Identifiers.Select(i => i.Value));
        }

        private string GetOperatorText(BooleanComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case BooleanComparisonType.Equals: return "=";
                case BooleanComparisonType.NotEqualToBrackets: return "<>";
                case BooleanComparisonType.NotEqualToExclamation: return "!=";
                case BooleanComparisonType.GreaterThan: return ">";
                case BooleanComparisonType.GreaterThanOrEqualTo: return ">=";
                case BooleanComparisonType.LessThan: return "<";
                case BooleanComparisonType.LessThanOrEqualTo: return "<=";
                default:
                    throw new NotSupportedException($"Неподдерживаемый тип сравнения: {comparisonType}");
            }
        }

        private string GenerateScript(TSqlFragment fragment)
        {
            var writer = new StringWriter();
            _scriptGenerator.GenerateScript(fragment, writer);
            return writer.ToString().Trim();
        }

        public SqlAnalysisResult GetResult()
        {
            return new SqlAnalysisResult
            {
                IsValid = true,
                TablesUsed = TablesUsed.Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
                Conditions = Conditions.Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
                SelectedColumns = SelectedColumns.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
            };
        }
    }
}

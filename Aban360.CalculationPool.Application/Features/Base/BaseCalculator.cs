using Aban360.CalculationPool.Domain.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using org.matheval;
using System.Reflection;

namespace Aban360.CalculationPool.Application.Features.Base
{
    internal abstract class BaseCalculator
    {
        private Dictionary<string, object> GetDictionaryOfProperties(object obj)
        {
            if (obj == null)
            {
                return new Dictionary<string, object>();
            }
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(obj, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
        internal void Bind(Expression expression, string formulaVariable, Dictionary<string, object> propertyDictionary)
        {
            foreach (var prop in propertyDictionary)
            {
                if (formulaVariable != null && formulaVariable.Equals(prop.Key))
                {
                    expression.Bind(prop.Key, prop.Value);
                    break;
                }
            }
        }
        internal Expression GetExpression(string formula, object info)
        {
            Dictionary<string, object> propertyDictionary = GetDictionaryOfProperties(info);
            Expression expression = new Expression(formula);
            List<string> errors = expression.GetError();
            if (errors != null && errors.Any())
            {
                throw new ExpressionValidationException(errors.First());
            }
            List<string> formulaVariables = expression.getVariables();
            foreach (string variable in formulaVariables)
            {
                Bind(expression, variable, propertyDictionary);
            }
            return expression;
        }
    }
}

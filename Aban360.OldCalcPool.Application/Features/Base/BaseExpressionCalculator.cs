using Aban360.OldCalcPool.Domain.Exceptions;
using org.matheval;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aban360.OldCalcPool.Application.Features.Base
{
    internal abstract class BaseExpressionCalculator
    {
        internal Expression GetExpression(string formula, object info, [Optional] Dictionary<string, object>? dependencyDictionary)
        {
            Dictionary<string, object> propertyDictionary = GetDictionaryOfProperties(info);
            if (dependencyDictionary is not null)
            {
                propertyDictionary = propertyDictionary
                    .Union(dependencyDictionary)
                    .ToDictionary();
            }
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
        private void Bind(Expression expression, string formulaVariable, Dictionary<string, object> propertyDictionary)
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
    }
}

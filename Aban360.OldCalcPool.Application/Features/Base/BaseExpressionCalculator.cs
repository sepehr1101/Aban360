using Aban360.OldCalcPool.Domain.Exceptions;
using DynamicExpresso;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aban360.OldCalcPool.Application.Features.Base
{
    internal abstract class BaseExpressionCalculator
    {
        internal T Eval<T>(string formula, object info, [Optional] Dictionary<string, object>? dependencyDictionary)
        {
            try
            {
                Dictionary<string, object> propertyDictionary = GetDictionaryOfProperties(info);
                if (dependencyDictionary is not null)
                {
                    propertyDictionary = propertyDictionary
                        .Union(dependencyDictionary)
                        .ToDictionary();
                }
                Interpreter interpreter = new();
                
                BindVariables(interpreter, propertyDictionary);
                return interpreter.Eval<T>(formula);
            }
            catch (Exception e)
            {
                throw new ExpressionValidationException(formula);
            }
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
        private void BindVariables(Interpreter expression, Dictionary<string, object> propertyDictionary)
        {
            foreach (var prop in propertyDictionary)
            {
                expression.SetVariable(prop.Key, prop.Value);
            }
        }
    }
}

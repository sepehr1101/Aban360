using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Aban360.CalculationPool.Application.Features.CalculationTest
{
    public class CalculationTest : ICalculationTest
    {
        public string Handle<D>(string formula, D entity)
        {
            formula=ReplaceInFormula(formula,entity);
            string result=CalcFormula(formula);

            return result;
        }

        private string ReplaceInFormula<D>(string formula,D entity)
        {
            string pattern = $@"{typeof(D).Name}\.(\w+)";
            MatchCollection matches = Regex.Matches(formula, pattern);

            foreach (Match match in matches)
            {
                string propertyName = match.Groups[1].Value; //Property

                PropertyInfo property = typeof(D).GetProperty(propertyName);
                if (property != null)
                {
                    object value = property.GetValue(entity);
                    formula = formula.Replace(match.Value, value.ToString());
                }
            }

            return formula;
        }

        private string CalcFormula(string formula)
        {

            DataTable dt = new DataTable();
            var result = dt.Compute(formula, "");

            return result.ToString();
        }
    }
}

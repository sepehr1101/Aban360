namespace org.matheval.Functions.Impl
{
    using global::org.matheval.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    namespace org.matheval.Functions
    {
        /// <summary>
        /// CONTAINS(list, value) -> true/false
        /// new Afe_Evaluator("CONTAINS(tags, 1)").Bind("tags", new List<decimal>{1,2,3}).Eval() -> true
        /// </summary>
        public class containsFunction : IFunction
        {
            /// <summary>
            /// Get Information
            /// </summary>
            /// <returns>FunctionDefs</returns>
            public List<FunctionDef> GetInfo()
            {
                return new List<FunctionDef> {
                // CONTAINS(list, value) -> bool
                new FunctionDef(Afe_Common.Const_Contains, new Type[] { typeof(object), typeof(object) }, typeof(bool), 2),
            };
            }

            /// <summary>
            /// Execute
            /// </summary>
            /// <param name="args">args</param>
            /// <param name="dc">dc</param>
            /// <returns>true if found, false otherwise</returns>
            public Object Execute(Dictionary<string, Object> args, ExpressionContext dc)
            {
                object listObj = args[Afe_Common.Const_Key_One];
                object value = args[Afe_Common.Const_Key_Two];

                if (listObj == null || value == null)
                    return false;

                // If it's a list (array, List<>, etc.)
                if (Afe_Common.IsList(listObj))
                {
                    foreach (object item in (IEnumerable)listObj)
                    {
                        if (EqualsNormalized(item, value, dc))
                            return true;
                    }
                    return false;
                }

                // If it's a string, check substring
                if (listObj is string s1 && value is string s2)
                {
                    return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                // Otherwise, compare single values
                return EqualsNormalized(listObj, value, dc);
            }

            /// <summary>
            /// Normalize equality: supports numeric/string comparisons
            /// </summary>
            private bool EqualsNormalized(object a, object b, ExpressionContext dc)
            {
                if (a == null || b == null)
                    return false;

                // If both are numbers
                if (Afe_Common.IsNumber(a) && Afe_Common.IsNumber(b))
                {
                    decimal da = Afe_Common.ToDecimal(a, dc.WorkingCulture);
                    decimal db = Afe_Common.ToDecimal(b, dc.WorkingCulture);
                    return da == db;
                }

                // Otherwise, compare as strings (ignore case)
                return string.Equals(a.ToString(), b.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }
    }

}

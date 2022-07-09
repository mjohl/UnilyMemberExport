namespace Unily.Member.Export.Lucene;

public sealed class LuceneFilterGroupBuilder
    {
        private readonly LuceneQueryPartRule _rule;
        private readonly bool _applyBrackets;
        private readonly List<string> _queryParts = new List<string>();

        internal LuceneFilterGroupBuilder(LuceneQueryPartRule rule, bool applyBrackets)
        {
            _rule = rule;
            _applyBrackets = applyBrackets;
        }

        /// <summary>
        /// Adds search text to the filter group
        /// </summary>
        /// <param name="searchText">The search text</param>
        public void AddSearchText(string searchText)
        {
            _queryParts.Add($"+{searchText}*");
        }

        /// <summary>
        /// Adds a filter to the filter group
        /// </summary>
        /// <param name="propertyAlias">The alias of the property to filter</param>
        /// <param name="value">The value to filter</param>
        /// <param name="rule">The rule of the filter</param>
        public void AddFilter(string propertyAlias, string value, LuceneQueryPartRule rule = LuceneQueryPartRule.MayMatch)
        {
            _queryParts.Add($"{rule.GetOperator()}{propertyAlias}:{value}");
        }

        /// <summary>
        /// Adds a filter sub-group
        /// </summary>
        /// <param name="buildFilterGroup">Build the filter group</param>
        /// <param name="rule">The rule of the filter group</param>
        public void AddFilterGroup(Action<LuceneFilterGroupBuilder> buildFilterGroup, LuceneQueryPartRule rule = LuceneQueryPartRule.MayMatch)
        {
            var filterGroupBuilder = new LuceneFilterGroupBuilder(rule, true);

            buildFilterGroup(filterGroupBuilder);

            _queryParts.Add(filterGroupBuilder.ToString());
        }

        /// <summary>
        /// Adds the isTrashed filter to the query group
        /// </summary>
        public void AddIsTrashedFilter()
        {
            AddFilter("isTrashed", "false", LuceneQueryPartRule.MustMatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyAlias"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="rule"></param>
        public void AddRangeFilter(string propertyAlias, string from, string to, LuceneQueryPartRule rule = LuceneQueryPartRule.MustMatch)
        {
            AddFilter(propertyAlias, $"[{from} TO {to}]", rule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyAlias"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="rule"></param>
        public void AddDateFilter(string propertyAlias, DateTime? from, DateTime? to, LuceneQueryPartRule rule = LuceneQueryPartRule.MustMatch)
        {
            const string luceneDateFormat = "yyyyMMddHHmmssfff";

            var fromString = (from ?? DateTime.MinValue).ToString(luceneDateFormat);
            var toString = (to ?? DateTime.MaxValue).ToString(luceneDateFormat);

            AddRangeFilter(propertyAlias, fromString, toString, rule);
        }

        /// <summary>
        /// Converts the Filter Group to a Lucene Query string
        /// </summary>
        /// <returns>The filter group in Lucene Query syntax</returns>
        public override string ToString()
        {
            var queryParts = string.Join(" ", _queryParts);
            
            switch (_rule)
            {
                case LuceneQueryPartRule.MustMatch:
                case LuceneQueryPartRule.MustNotMatch:
                    var q = _rule.GetOperator();
                    if (_applyBrackets)
                        q += "(";

                    q += queryParts;

                    if (_applyBrackets)
                        q += ")";

                    queryParts = q;
                    break;
                case LuceneQueryPartRule.MayMatch:
                    break;
                default:
                    throw new NotImplementedException($"{nameof(LuceneFilterGroupBuilder)} doesn't implement a .ToString() route for {nameof(LuceneQueryPartRule)}.{_rule}");
            }

            return queryParts;
        }
    }
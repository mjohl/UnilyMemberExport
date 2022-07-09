namespace Unily.Member.Export.Lucene;

public sealed class LuceneQueryBuilder
{
    /// <summary>
    /// Executes the buildQuery action and returns a compiled Lucene query
    /// </summary>
    /// <param name="buildQuery">Build the Lucene query</param>
    /// <param name="rule">The rule to which the top-level filter group should adhere to</param>
    /// <param name="options">Allows configuration of additional options</param>
    /// <returns>The compiled Lucene Query</returns>
    public static string GetQueryText(Action<LuceneFilterGroupBuilder> buildQuery, LuceneQueryPartRule rule = LuceneQueryPartRule.MayMatch)
    {
        var topLevelFilterGroupBuilder = new LuceneFilterGroupBuilder(rule, rule != LuceneQueryPartRule.MayMatch);

        buildQuery(topLevelFilterGroupBuilder);

        return topLevelFilterGroupBuilder.ToString();
    }
}
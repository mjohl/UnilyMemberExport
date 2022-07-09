namespace Unily.Member.Export.Lucene;

internal static class LuceneQueryPartRuleExtensions
{
    internal static string GetOperator(this LuceneQueryPartRule rule)
    {
        var @operator = string.Empty;

        switch (rule)
        {
            case LuceneQueryPartRule.MayMatch:
                break;
            case LuceneQueryPartRule.MustMatch:
                @operator = "+";
                break;
            case LuceneQueryPartRule.MustNotMatch:
                @operator = "-";
                break;
            default:
                throw new NotImplementedException($"{nameof(LuceneQueryPartRule)}.{rule} doesn't have an operator implemented");
        }

        return @operator;
    }
}
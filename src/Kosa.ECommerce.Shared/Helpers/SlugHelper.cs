using System.Text;

namespace Kosa.ECommerce.Shared.Helpers;

/// <summary>
/// Generates URL friendly slugs from entity display names.
/// Used by the catalog (products / categories) so API consumers can
/// address records by slug instead of by integer id.
/// </summary>
public static class SlugHelper
{
    public static string CreateSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var result = new StringBuilder(value.Trim().ToLowerInvariant().Length);

        var lastWasDash = false;

        foreach (var ch in value.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(ch))
            {
                result.Append(ch);
                lastWasDash = false;
            }
            else if (ch == '-' && !lastWasDash && result.Length > 0)
            {
                result.Append('-');
                lastWasDash = true;
            }
            else if (char.IsWhiteSpace(ch) && result.Length > 0 && !lastWasDash)
            {
                result.Append('-');
                lastWasDash = true;
            }
        }

        return result.ToString().Trim('-');
    }
}

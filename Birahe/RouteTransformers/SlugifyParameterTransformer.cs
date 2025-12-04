using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Birahe.EndPoint.RouteTransformers {
    public class SlugifyParameterTransformer : IOutboundParameterTransformer {
        public string? TransformOutbound(object? value) {
            if (value == null) return null;

            // Convert PascalCase → kebab-case
            return Regex.Replace(
                value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2"
            ).ToLower();
        }
    }
}
namespace OmniXaml.Tests.XmlParserTests
{
    using System.Reflection;
    using Namespaces;
    using Services;
    using TypeLocation;

    public class XamlToTreeParserTestsBase
    {
        protected static ParseResult ParseResult(string xaml)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
           
            var namespaces = new[]
            {
                XamlNamespace.Map("root").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model")),
                XamlNamespace.Map("custom").With(Route.Assembly(ass).WithNamespaces("OmniXaml.Tests.Model.Custom")),
            };

            var directory = new TypeDirectory(namespaces);

            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(new AttributeBasedMetadataProvider(), new[] {new InlineParser(resolver) }, resolver);

            var prefixAnnotator = new PrefixAnnotator();
            var tree = sut.Parse(xaml, prefixAnnotator );
            return tree;
        }
    }
}
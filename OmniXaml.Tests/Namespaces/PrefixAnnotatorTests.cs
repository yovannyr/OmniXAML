﻿namespace OmniXaml.Tests.Namespaces
{
    using System.Collections.Generic;
    using System.Reflection;
    using Model;
    using Model.Custom;
    using TypeLocation;
    using Xunit;

    public class PrefixAnnotatorTests
    {
        [Fact]
        public void ParentPrefixesAreAvailableInChild()
        {
            var type = typeof(TextBlock);
            var assembly = type.GetTypeInfo().Assembly;
            var xamlNamespaces = XamlNamespace.Map("root").With(Route.Assembly(assembly).WithNamespaces(type.Namespace));
            var prefixAnnotator = new PrefixAnnotator();

            var sut = new PrefixedTypeResolver(prefixAnnotator, new TypeDirectory(new[] { xamlNamespaces }));

            var childNode = new ConstructionNode(typeof(TextBlock));
            var root = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>()
                {
                    new MemberAssignment()
                    {
                        Member = Member.FromStandard<Window>(window => window.Content),
                        Children = new[] {childNode,}
                    }
                },
            };

            prefixAnnotator.Annotate(root, new List<PrefixDeclaration>() { new PrefixDeclaration(string.Empty, "root") });

            sut.Root = root;
            var buttonType = sut.GetTypeByPrefix(childNode, "Button");
            Assert.Equal(typeof(Button), buttonType);
        }

        [Fact]
        public void TestMethod2()
        {
            var prefixAnnotator = new PrefixAnnotator();

            var newTypeDirectory = GetTypeDirectory();

            var sut = new PrefixedTypeResolver(prefixAnnotator, newTypeDirectory);

            var childNode = new ConstructionNode(typeof(TextBlock));
            var root = new ConstructionNode(typeof(Window))
            {
                Assignments = new List<MemberAssignment>() { new MemberAssignment()
                {
                    Member = Member.FromStandard<Window>(window => window.Content),
                    Children = new[] { childNode, }
                } },                
            };

            prefixAnnotator.Annotate(childNode, new List<PrefixDeclaration>() { new PrefixDeclaration("a", "another") });

            sut.Root = root;
            var customGridType = sut.GetTypeByPrefix(childNode, "a:CustomGrid");
            Assert.Equal(typeof(CustomGrid), customGridType);
        }

        private static TypeDirectory GetTypeDirectory()
        {
            var type = typeof(TextBlock);
            var typeAnother = typeof(CustomControl);

            var assembly = type.GetTypeInfo().Assembly;
            var nsAnother = XamlNamespace.Map("another").With(Route.Assembly(assembly).WithNamespaces(typeAnother.Namespace));

            var newTypeDirectory = new TypeDirectory(new[] {nsAnother});
            return newTypeDirectory;
        }
    }
}

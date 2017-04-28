﻿namespace OmniXaml.Services
{
    using ReworkPhases;

    public class FullObjectBuilder : IFullObjectBuilder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier memberAssigmentApplier;

        public FullObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier memberAssigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.memberAssigmentApplier = memberAssigmentApplier;
        }

        public object Build(ConstructionNode node)
        {
            var mainBuilder = new ObjectBuilder(instanceCreator, converter, memberAssigmentApplier);
            var unresolvedFixer = new ObjectBuilderSecondPass(converter);

            var inflatedNode = mainBuilder.Build(node);
            return unresolvedFixer.Fix(inflatedNode);
        }
    }
}
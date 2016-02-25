//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidatorBuilderBase
    {
        private readonly MemberAccessValidatorBuilderFactory _memberAccessValidatorFactory;
        private readonly ValidatorFactory _validatorFactory;

        public ValidatorBuilderBase(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory,
                                    ValidatorFactory validatorFactory)
        {
            _memberAccessValidatorFactory = memberAccessValidatorFactory;
            _validatorFactory = validatorFactory;
        }
        
        public Validator CreateValidator(IValidatedType validatedType)
        {
            var validators = new List<Validator>();

            CollectValidatorsForType(validatedType, validators);
            CollectValidatorsForProperties(validatedType.GetValidatedProperties(), validators);
            CollectValidatorsForFields(validatedType.GetValidatedFields(), validators);
            CollectValidatorsForMethods(validatedType.GetValidatedMethods(), validators);
            CollectValidatorsForSelfValidationMethods(validatedType.GetSelfValidationMethods(), validators);

            if (validators.Count == 1)
            {
                return validators[0];
            }
            return new AndCompositeValidator(validators.ToArray());
        }

        private void CollectValidatorsForType(IValidatedType validatedType, List<Validator> validators)
        {
            Validator validator = CreateValidatorForValidatedElement(validatedType, GetCompositeValidatorBuilderForType);

            if (validator != null)
            {
                validators.Add(validator);
            }
        }

        private void CollectValidatorsForProperties(IEnumerable<IValidatedElement> validatedElements,
                                                    List<Validator> validators)
        {
            foreach (IValidatedElement validatedElement in validatedElements)
            {
                Validator validator = CreateValidatorForValidatedElement(validatedElement,
                                                                         GetCompositeValidatorBuilderForProperty);

                if (validator != null)
                {
                    validators.Add(validator);
                }
            }
        }

        private void CollectValidatorsForFields(IEnumerable<IValidatedElement> validatedElements,
                                                List<Validator> validators)
        {
            foreach (IValidatedElement validatedElement in validatedElements)
            {
                Validator validator = CreateValidatorForValidatedElement(validatedElement,
                                                                         GetCompositeValidatorBuilderForField);

                if (validator != null)
                {
                    validators.Add(validator);
                }
            }
        }

        private void CollectValidatorsForMethods(IEnumerable<IValidatedElement> validatedElements,
                                                 List<Validator> validators)
        {
            foreach (IValidatedElement validatedElement in validatedElements)
            {
                Validator validator = CreateValidatorForValidatedElement(validatedElement, GetCompositeValidatorBuilderForMethod);

                if (validator != null)
                {
                    validators.Add(validator);
                }
            }
        }

        private void CollectValidatorsForSelfValidationMethods(IEnumerable<MethodInfo> selfValidationMethods, List<Validator> validators)
        {
            foreach (MethodInfo selfValidationMethod in selfValidationMethods)
            {
                validators.Add(new SelfValidationValidator(selfValidationMethod));
            }
        }

        protected Validator CreateValidatorForValidatedElement(IValidatedElement validatedElement,
                                                               CompositeValidatorBuilderCreator validatorBuilderCreator)
        {
            IEnumerator<IValidatorDescriptor> validatorDescriptorsEnumerator =
                validatedElement.GetValidatorDescriptors().GetEnumerator();

            if (!validatorDescriptorsEnumerator.MoveNext())
            {
                return null;
            }

            CompositeValidatorBuilder validatorBuilder = validatorBuilderCreator(validatedElement);

            do
            {
                Validator validator = validatorDescriptorsEnumerator.Current.CreateValidator(validatedElement.TargetType,
                                                                                             validatedElement.MemberInfo.ReflectedType,
                                                                                            _memberAccessValidatorFactory.MemberValueAccessBuilder,
                                                                                            _validatorFactory);
                validatorBuilder.AddValueValidator(validator);
            }
            while (validatorDescriptorsEnumerator.MoveNext());

            return validatorBuilder.GetValidator();
        }
        
        protected delegate CompositeValidatorBuilder CompositeValidatorBuilderCreator(IValidatedElement validatedElement);
        
        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
        {
            if (validatedElement == null) throw new ArgumentNullException("validatedElement");

            return _memberAccessValidatorFactory.GetPropertyValueAccessValidatorBuilder(validatedElement.MemberInfo as PropertyInfo,
                validatedElement);
        }
        
        protected CompositeValidatorBuilder GetValueCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
        {
            return new CompositeValidatorBuilder(validatedElement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatedElement"></param>
        /// <returns></returns>
        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForField(IValidatedElement validatedElement)
        {
            if (validatedElement == null) throw new ArgumentNullException("validatedElement");

            return _memberAccessValidatorFactory.GetFieldValueAccessValidatorBuilder(validatedElement.MemberInfo as FieldInfo,
                validatedElement);
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForMethod(IValidatedElement validatedElement)
        {
            if (validatedElement == null) throw new ArgumentNullException("validatedElement");

            return _memberAccessValidatorFactory.GetMethodValueAccessValidatorBuilder(validatedElement.MemberInfo as MethodInfo, validatedElement);
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForType(IValidatedElement validatedElement)
        {
            if (validatedElement == null) throw new ArgumentNullException("validatedElement");

            return _memberAccessValidatorFactory.GetTypeValidatorBuilder(validatedElement.MemberInfo as Type, validatedElement);
        }

        protected ValidatorFactory ValidatorFactory
        {
            get { return _validatorFactory; }
        }
    }
}

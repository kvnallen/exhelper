using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;
using static System.Net.Mime.MediaTypeNames;

namespace ExHelper.Tests
{
    public sealed class EmbeddedResourceDataAttribute : DataAttribute
    {
        private readonly string[] _args;

        public EmbeddedResourceDataAttribute(params string[] args)
        {
            _args = args;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var result = new object[_args.Length];
            for (var index = 0; index < _args.Length; index++)
            {
                result[index] = ReadManifestData(_args[index]);
            }
            return new[] { result };
        }

        public static Stream ReadManifestData(string resourceName)
        {
            var assembly = typeof(EmbeddedResourceDataAttribute).GetTypeInfo().Assembly;
            resourceName = resourceName.Replace("/", ".");
            var stream = assembly.GetManifestResourceStream(resourceName);


            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }

            return stream;
        }
    }
}
using JsonPatch.Restrict.Tests.Models;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace JsonPatch.Restrict.Tests
{
    public class RestrictionTests
    {
        [Fact]
        public void ApplyRestrictedPath()
        {
            var dummy = new DummyModel
            {
                Id = 1,
                Value = "baseValue",
            };

            GetPatchDocument().ApplyToWithRestrictions(dummy, "Value");

            Assert.Equal(1, dummy.Id);
            Assert.Equal("newValue", dummy.Value);
        }

        [Fact]
        public void TryApplyRestrictedPath()
        {
            var dummy = new DummyModel
            {
                Id = 1,
                Value = "baseValue",
            };

            Assert.True(GetPatchDocument().TryApplyToWithRestrictions(dummy, out _, "Value"));

            Assert.Equal(1, dummy.Id);
            Assert.Equal("newValue", dummy.Value);
        }

        [Fact]
        public void TryApplyIllegalRestrictedPath()
        {
            var dummy = new DummyModel
            {
                Id = 1,
                Value = "baseValue",
            };

            Assert.False(GetPatchDocument().TryApplyToWithRestrictions(dummy, out var error));

            Assert.NotNull(error);

            Assert.Equal(1, dummy.Id);
            Assert.Equal("baseValue", dummy.Value);
        }

        [Fact]
        public void ApplyIllegalRestrictedPath()
        {
            var dummy = new DummyModel
            {
                Id = 1,
                Value = "baseValue",
            };

            JsonPatchError? error = default;

            GetPatchDocument().ApplyToWithRestrictions(dummy, e => error = e);

            Assert.NotNull(error);

            Assert.Equal(1, dummy.Id);
            Assert.Equal("baseValue", dummy.Value);
        }

        [Fact]
        public void ApplyPatchToChildProperty()
        {
            var oldValue = "oldValue";
            var newValue = "newValue";
            
            var dummyWithChild = new DummyModelWithChild
            {
                Id = 1,
                Child = new Child
                {
                    Value = oldValue,  
                },
            };
            
            var patchDocument = new JsonPatchDocument();
            patchDocument.Replace("/Child/Value", newValue);
            patchDocument.ApplyToWithRestrictions(dummyWithChild, "/Child/Value");
            
            Assert.Equal(1, dummyWithChild.Id);
            Assert.Equal(newValue, dummyWithChild.Child.Value);
        }

        [Fact]
        public void ApplyPatchToNestedChildProperty()
        {
            var oldValue = "oldValue";
            var newValue = "newValue";

            var dummy = new DummyMultiChild
            {
                Id = 1,
                Child1 = new Child1
                {
                    Child2 = new Child2
                    {
                        Value  = oldValue,
                    },
                },
            };
            
            var patchDocument = new JsonPatchDocument();
            patchDocument.Replace("/Child1/Child2/Value", newValue);
            patchDocument.ApplyToWithRestrictions(dummy, "/Child1/Child2/Value");
            
            Assert.Equal(1, dummy.Id);
            Assert.Equal(newValue, dummy.Child1.Child2.Value);
        }

        private JsonPatchDocument GetPatchDocument()
        {
            var patchDocument = new JsonPatchDocument();

            patchDocument.Replace("/Value", "newValue");

            return patchDocument;
        }
    }
}

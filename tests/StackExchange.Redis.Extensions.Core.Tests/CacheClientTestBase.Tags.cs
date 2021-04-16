﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StackExchange.Redis.Extensions.Core.Helpers;
using StackExchange.Redis.Extensions.Core.Models;
using StackExchange.Redis.Extensions.Tests.Helpers;

using Xunit;

namespace StackExchange.Redis.Extensions.Core.Tests
{
    public abstract partial class CacheClientTestBase
    {
        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsync_WithTags_ShouldAdd()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTags = new HashSet<string> { "test_tag" };

            var addResult = await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: testTags);

            Assert.True(addResult);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsync_WithTags_TagExists()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: new HashSet<string> { testTag });

            var tagExists = await db.KeyExistsAsync(TagHelper.GenerateTagKey(testTag));

            Assert.True(tagExists);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsync_WithTags_CorrectTaggedKey()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: new HashSet<string> { testTag });

            var tags = await db.SetMembersAsync(TagHelper.GenerateTagKey(testTag));
            var deserialized = tags?.Length > 0 ? serializer.Deserialize<string>(tags[0]) : string.Empty;

            Assert.Equal(testKey, deserialized);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncTimeSpan_WithTags_ShouldAdd()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTags = new HashSet<string> { "test_tag" };

            var addResult = await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, TimeSpan.FromSeconds(1), tags: testTags);

            Assert.True(addResult);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncTimeSpan_WithTags_TagExists()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, TimeSpan.FromSeconds(1), tags: new HashSet<string> { testTag });

            var tagExists = await db.KeyExistsAsync(TagHelper.GenerateTagKey(testTag));

            Assert.True(tagExists);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncTimeSpan_WithTags_CorrectTaggedKey()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, TimeSpan.FromSeconds(1), tags: new HashSet<string> { testTag });

            var tags = await db.SetMembersAsync(TagHelper.GenerateTagKey(testTag));
            var deserialized = serializer.Deserialize<string>(tags[0]);

            Assert.Equal(testKey, deserialized);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncDateTimeOffset_WithTags_ShouldAdd()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTags = new HashSet<string> { "test_tag" };

            var addResult = await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, DateTimeOffset.UtcNow, tags: testTags);

            Assert.True(addResult);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncDateTimeOffset_WithTags_TagExists()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, DateTimeOffset.UtcNow, tags: new HashSet<string> { testTag });

            var tagExists = await db.KeyExistsAsync(TagHelper.GenerateTagKey(testTag));

            Assert.True(tagExists);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task AddAsyncDateTimeOffset_WithTags_CorrectTaggedKey()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, DateTimeOffset.UtcNow, tags: new HashSet<string> { testTag });

            var tags = await db.SetMembersAsync(TagHelper.GenerateTagKey(testTag));
            var deserialized = serializer.Deserialize<string>(tags[0]);

            Assert.Equal(testKey, deserialized);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task GetByTagAsync_ShouldReturnSomeValues()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: new HashSet<string> { testTag });

            var result = await Sut.GetDbFromConfiguration().GetByTagAsync<TestClass<string>>(testTag);

            Assert.Equal(1, result?.Count());
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task GetByTagAsync_ShouldReturnCorrectValue()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: new HashSet<string> { testTag });

            var result = await Sut.GetDbFromConfiguration().GetByTagAsync<TestClass<string>>(testTag);

            Assert.Equal(testClass, result.First());
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task RemoveByTagAsync_ShouldReturnZero()
        {
            var testTag = "test_tag";

            var result = await Sut.GetDbFromConfiguration().RemoveByTagAsync(testTag);

            Assert.Equal(0, result);
        }

        [Fact]
        [Trait("Category", "Tags")]
        public async Task RemoveByTagAsync_ShouldReturnOneDeletedValue()
        {
            var testKey = "test_key";
            var testValue = "test_value";
            var testClass = new TestClass<string>(testKey, testValue);
            var testTag = "test_tag";

            await Sut.GetDbFromConfiguration().AddAsync(testKey, testClass, tags: new HashSet<string> { testTag });

            var result = await Sut.GetDbFromConfiguration().RemoveByTagAsync(testTag);

            Assert.Equal(1, result);
        }
    }
}

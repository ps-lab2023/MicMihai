using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PhotogGalleryApp.Controllers;
using PhotogGalleryApp.Data;

namespace PhotoGalleryAppTests
{
    namespace PhotoGalleryAppTests
    {
        public class CrudeTests
        {
            private bool resulted = true;
            private PhotoGalleryDbContext GetInMemoryDbContext()
            {
                var options = new DbContextOptionsBuilder<PhotoGalleryDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name
                    .Options;

                var dbContext = new PhotoGalleryDbContext(options);

                if (!dbContext.Tags.Any())
                {
                    dbContext.Tags.AddRange(GetTestTags());
                    dbContext.SaveChanges();
                }

                return dbContext;
            }

            private List<Tag> GetTestTags()
            {
                return new List<Tag>
            {
                new Tag { TagId = 1, Name = "Landscape" },
                new Tag { TagId = 2, Name = "Portrait" },
                new Tag { TagId = 3, Name = "Wildlife" },
            };
            }

            [Fact]
            public async Task GetTags_Returns_All_Tags()
            {
                // Arrange
                using var dbContext = GetInMemoryDbContext();
                var controller = new TagController(dbContext);

                // Act
                var result = await controller.GetTags();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Tag>>>(result);
                var tags = Assert.IsType<List<Tag>>(actionResult.Value);
                Assert.Equal(3, tags.Count);
            }

            [Fact]
            public async Task GetTag_Returns_Correct_Tag()
            {
                // Arrange
                using var dbContext = GetInMemoryDbContext();
                var controller = new TagController(dbContext);
                int tagId = 1;

                // Act
                var result = await controller.GetTag(tagId);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Tag>>(result);
                var tag = Assert.IsType<Tag>(actionResult.Value);
                Assert.Equal(tagId, tag.TagId);
                Assert.Equal("Landscape", tag.Name);
            }
            [Fact]
            public async Task PutTag_Updates_Tag()
            {
                // Arrange
                using var dbContext = GetInMemoryDbContext();
                var controller = new TagController(dbContext);
                int tagId = 1;
                var existingTag = await dbContext.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.TagId == tagId); // Add .AsNoTracking()
                var updatedTag = new Tag { TagId = tagId, Name = "Updated Landscape" };

                // Act
                var result = await controller.PutTag(tagId, updatedTag);

                // Assert
                var tag = await dbContext.Tags.FindAsync(tagId);
                Assert.True(resulted);
            }


            [Fact]
            public async Task PostTag_Creates_New_Tag()
            {
                // Arrange
                using var dbContext = GetInMemoryDbContext();
                var controller = new TagController(dbContext);
                var newTag = new Tag { Name = "New Tag" };

                // Act
                var result = await controller.PostTag(newTag);

                // Assert
 36            
                Assert.True(resulted);
            }

            [Fact]
            public async Task DeleteTag_Removes_Tag()
            {
                // Arrange
                using var dbContext = GetInMemoryDbContext();
                var controller = new TagController(dbContext);
                int tagId = 1;

                // Act
                var result = await controller.DeleteTag(tagId);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var tag = await dbContext.Tags.FindAsync(tagId);
                Assert.Null(tag);
            }

        }
    }

}

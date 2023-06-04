using eFilingAPI.Models;
using eFilingAPI.Repositories;
using AutoWrapper.Exceptions;
using Microsoft.Extensions.Hosting;
using AutoWrapper.Attributes;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using System.IO;

namespace eFilingAPI.Services
{
    public class PostService
    {
        private readonly DataContext db;

        public PostService(DataContext db)
        {
            this.db = db;
        }

        public Post Get(int id)
        {
            return db.Post.Find(id);
        }

        public List<Post> GetList()
        {
            return db.Post.ToList();
        }

        public Post Create(Post post)
        {
            db.Post.Add(post);
            db.SaveChanges();
            return post;
        }

        public Post Update(Post post)
        {
            db.Post.Update(post);
            db.SaveChanges();
            return post;
        }

        public void Delete(int id)
        {
            var post = db.Post.Find(id);
            db.Post.Remove(post);
            db.SaveChanges();
        }

        public async Task<AttachmentFile> UploadFile(AttachmentFile model)
        {
            try
            {

                #region Save File
                string path = Path.Combine(System.Environment.CurrentDirectory, "Uploads");

                List<string> uploadedFiles = new List<string>();
                string ext = Path.GetExtension(model.File.FileName);

                #region Create Data
                Post post = new Post
                {
                    Type = "File",
                    FileName = model.FileName,
                    FileExt = ext,
                    PathFile = path,
                    DateCreated = DateTime.Now
                };

                db.Post.Add(post);
                db.SaveChanges();
                int contentId = post.Id;
                #endregion Create Data

                string filename = contentId + ext;
                var Destination = Path.Combine(path, filename);

                if (model.File.Length > 0)
                {
                    using (Stream fileStream = new FileStream(Destination, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }
                }
                #endregion Save File

                return model;
            }
            catch (Exception ex)
            {
                throw new ApiException("error create UploadFile", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<AttachmentFileUpdate> UpdateFile(AttachmentFileUpdate model)
        {
            try
            {

                #region Save File
                string path = Path.Combine(System.Environment.CurrentDirectory, "Uploads");

                List<string> uploadedFiles = new List<string>();
                string ext = Path.GetExtension(model.File.FileName);

                #region Create Data
                Post post = new Post
                {
                    Id = model.Id,
                    Type = "File",
                    FileName = model.FileName,
                    FileExt = ext,
                    PathFile = path,
                    DateCreated = DateTime.Now
                };

                db.Post.Update(post);
                db.SaveChanges();
                int contentId = post.Id;
                #endregion Create Data

                string filename = contentId + ext;
                var Destination = Path.Combine(path, filename);

                if (model.File.Length > 0)
                {
                    using (Stream fileStream = new FileStream(Destination, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }
                }
                #endregion Save File

                return model;
            }
            catch (Exception ex)
            {
                throw new ApiException("error create UploadFile", StatusCodes.Status400BadRequest);
            }
        }
    }
}

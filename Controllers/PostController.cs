using System;
using System.IO;
using eFilingAPI.Models;
using eFilingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using AutoWrapper.Exceptions;
using AutoWrapper.Attributes;

namespace eFilingAPI.Controllers;

[Route("post")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly PostService postService;

    public PostController(PostService postService)
    {
        this.postService = postService;
    }

    [HttpGet("FileInfo/{id}")]
    public ActionResult<Post> Get(int id)
    {
        var result = postService.Get(id);
        return Ok(result);
    }

    [HttpGet("ListFile")]
    public ActionResult<List<Post>> GetList()
    {
        var result = postService.GetList();
        return Ok(result);
    }

    [HttpPost("UploadFileAttachment"), DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
    [ProducesResponseType(typeof(AttachmentFile), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFileAttachment([FromForm] AttachmentFile model)
    {
        try
        {
            using (var op = Operation.Begin("UploadFile with {model}", model))
            {

                var data = await postService.UploadFile(model);


                op.Complete();
                return Ok(data);
            }
        }
        catch (Exception ex)
        {
            throw new ApiException(ex, StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet("DownloadFile/{id}")]
    public async Task<IActionResult> DownloadFile2(long id)
    {
        try
        {
            var Content = postService.Get(Convert.ToInt32(id));
            var FileName = Content.Id + Content.FileExt;
            var filePath = Path.Combine(
                   Directory.GetCurrentDirectory(),
                   "Uploads", id + Content.FileExt);  //get the file
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest();
            }
            return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", FileName);
        }
        catch (Exception ex)
        {
            throw new ApiException(ex, StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("UpdateFileAttachment"), DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
    [ProducesResponseType(typeof(AttachmentFileUpdate), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFileAttachment([FromForm] AttachmentFileUpdate model)
    {
        try
        {
            using (var op = Operation.Begin("UploadFile with {model}", model))
            {

                var data = await postService.UpdateFile(model);


                op.Complete();
                return Ok(data);
            }
        }
        catch (Exception ex)
        {
            throw new ApiException(ex, StatusCodes.Status400BadRequest);
        }
    }

    [HttpDelete("DeleteFile/{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var Content = postService.Get(Convert.ToInt32(id));
            var rootFolder = Content.PathFile.ToString();
            var FileName = Content.Id + Content.FileExt;
            var filePath = Path.Combine(
                       Directory.GetCurrentDirectory(),
                       "Uploads", id + Content.FileExt);  //get the file
            System.IO.File.Delete(filePath);

            postService.Delete(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            throw new ApiException(ex, StatusCodes.Status400BadRequest);
        }
    }
}
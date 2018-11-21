using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using mytvlistapi.Helpers;
using mytvlistapi.Models;

namespace mytvlistapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvListController : ControllerBase
    {
        private readonly mytvlistapiContext _context;
        private IConfiguration _configuration;

        public TvListController(mytvlistapiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/TvList
        [HttpGet]
        public IEnumerable<TvItem> GetTvItem()
        {
            return _context.TvItem;
        }
        // GET: api/TvList/Tags

        [HttpGet]
        [Route("tag")]
        public async Task<List<TvItem>> GetTagsItem([FromQuery] string tags)
        {
            var list = from m in _context.TvItem
                        select m; //get all the tv shows


            if (!String.IsNullOrEmpty(tags)) //make sure user gave a tag to search
            {
                list = list.Where(s => s.Tags.ToLower().Equals(tags.ToLower())); // find the entries with the search tag and reassign
            }

            var returned = await list.ToListAsync(); //return the tv shows

            return returned;
        }

        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]TvListImage Tvimage)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = Tvimage.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(Tvimage.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }

                    TvItem tvItem = new TvItem();
                    tvItem.Title = Tvimage.Title;
                    tvItem.Tags = Tvimage.Tags;
                    tvItem.Score = Tvimage.Score;

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    tvItem.Height = image.Height.ToString();
                    tvItem.Width = image.Width.ToString();
                    tvItem.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;

                    _context.TvItem.Add(tvItem);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {Tvimage.Title} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("list");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
        }


        // GET: api/TvList/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTvItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tvItem = await _context.TvItem.FindAsync(id);

            if (tvItem == null)
            {
                return NotFound();
            }

            return Ok(tvItem);
        }

        // PUT: api/TvList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTvItem([FromRoute] int id, [FromBody] TvItem tvItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tvItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(tvItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TvItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TvList
        [HttpPost]
        public async Task<IActionResult> PostTvItem([FromBody] TvItem tvItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TvItem.Add(tvItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTvItem", new { id = tvItem.Id }, tvItem);
        }

        // DELETE: api/TvList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTvItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tvItem = await _context.TvItem.FindAsync(id);
            if (tvItem == null)
            {
                return NotFound();
            }

            _context.TvItem.Remove(tvItem);
            await _context.SaveChangesAsync();

            return Ok(tvItem);
        }

        private bool TvItemExists(int id)
        {
            return _context.TvItem.Any(e => e.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Talking.Api.Data;
using Talking.Api.Repository;

namespace Talking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string post_url, [FromQuery] int? page, [FromQuery] int? limit)
        {
            try
            {
                // var ip = HttpContext.Connection.RemoteIpAddress.ToString();

                var data = string.IsNullOrEmpty(post_url)
                    ? _commentRepository.GetComments() // TODO: 不能查询所有
                    : _commentRepository.GetComments(post_url);
                return Ok(new { code = 0, data });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { code = 5, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            try
            {
                _commentRepository.InsertComment(comment);
                return Ok(comment.ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

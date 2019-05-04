using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Talking.Api.Data;
using Talking.Api.Models;
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
        public IActionResult Get([FromQuery] string post_url,
                                 [FromQuery] int? page,
                                 [FromQuery] int limit = 10)
        {
            try
            {
                IList<Comment> list;
                long total = 0;

                if (string.IsNullOrEmpty(post_url))
                {
                    list = _commentRepository.GetComments(); // TODO: 不能查询所有
                    total = list.Count;
                    return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
                }

                total = _commentRepository.GetCount(post_url);

                list = !page.HasValue ?
                    _commentRepository.GetComments(post_url) :
                    _commentRepository.GetComments(post_url, page.Value, limit);

                return Ok(HttpResponseFactory.CreateOk(detail: new { total, list }));
            }
            catch (System.Exception ex)
            {
                return BadRequest(HttpResponseFactory.CreateKo(
                    code: 5,
                    message: "exception",
                    detail: ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Comment comment)
        {
            try
            {
                var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                    ip = HttpContext.Connection.RemoteIpAddress.ToString();
                comment.Owner.IPv4 = ip;
                _commentRepository.InsertComment(comment);
                return Ok(HttpResponseFactory.CreateOk(message: "created", detail: comment));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseFactory.CreateKo(
                    code: 5,
                    message: "exception",
                    detail: ex.Message));
            }
        }
    }
}

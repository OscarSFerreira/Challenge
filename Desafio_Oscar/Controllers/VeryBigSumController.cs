using Desafio_Oscar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Oscar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeryBigSumController : ControllerBase
    {

        private readonly DataContext context;

        public VeryBigSumController(DataContext context)
        {
            this.context = context;
        }

        private static long Summary(VeryBigSum sum)
        {
            List<long> numarray = sum.Input.TrimEnd().Split(' ').ToList().Select(temp => Convert.ToInt64(temp)).ToList();

            long result = numarray.Sum();

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            VeryBigSum bigsums = new();
            if (bigsums.Id == System.Guid.Empty)
            {
                return NoContent();
            }

            try
            {
                bigsums.Input = input;
                bigsums.Output = Summary(bigsums).ToString();

                context.VeryBigSums.Add(bigsums);
                context.SaveChanges();

                return Ok(bigsums);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            
        }

        [HttpGet]
        public IActionResult Get()
        {

            try
            {
                var bigsum = context.VeryBigSums.ToList().OrderByDescending(sum => sum.Date);

                if (bigsum.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(bigsum);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {

            try
            {
                var bigsum = context.VeryBigSums.FirstOrDefault(sum => sum.Id == id);

                if (bigsum == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(bigsum);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {

            if (id == System.Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var bigsum = context.VeryBigSums.FirstOrDefault(sum => sum.Id == id);
                context.VeryBigSums.Remove(bigsum);
                context.SaveChanges();
                return Ok(true);
            }
            catch (Exception)
            {
                return NoContent();
            }

        }

    }

}

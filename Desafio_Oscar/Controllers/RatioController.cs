using Desafio_Oscar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Oscar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatioController : ControllerBase
    {

        private readonly DataContext context;

        public RatioController(DataContext context)
        {
            this.context = context;
        }

        private static string Summary(Ratio sum)
        {
            List<int> numarray = sum.Input.TrimEnd().Split(' ').ToList().Select(temp => Convert.ToInt32(temp)).ToList();

            decimal quantity = numarray.Count(), positives = 0, zeros = 0, negatives = 0;

            foreach (var item in numarray)
            {

                if (item > 0)
                {
                    positives++;
                }
                else if (item < 0)
                {
                    negatives++;
                }
                else if (item == 0)
                {
                    zeros++;
                }

            }

            var result = $"{(positives / numarray.Count).ToString("N6")} {(negatives / numarray.Count).ToString("N6")} {(zeros / numarray.Count).ToString("N6")}";

            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            Ratio ratio = new();

            if (ratio.Id == System.Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                ratio.Input = input;
                ratio.Output = Summary(ratio).ToString();

                context.Ratios.Add(ratio);
                context.SaveChanges();

                return Ok(ratio);
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
                var ratio = context.Ratios.ToList().OrderByDescending(rat => rat.Date);

                if (ratio.Count() == 0)
                {
                    return NoContent();
                }

                return Ok(ratio);

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
                var ratio = context.Ratios.FirstOrDefault(rat => rat.Id == id);

                if (ratio == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(ratio);
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
                var ratio = context.Ratios.FirstOrDefault(rat => rat.Id == id);
                context.Ratios.Remove(ratio);
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

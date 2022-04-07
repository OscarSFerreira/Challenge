using Desafio_Oscar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Oscar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagonalSumController : ControllerBase
    {

        private readonly DataContext context;

        public DiagonalSumController(DataContext context)
        {
            this.context = context;
        }

        private static int Summary(/*DiagonalSum sum*/ List<List<int>> arr)
        {

            int leftDiag = 0, rightDiag = 0;

            for (int i = 0; i < arr.Count; i++)
            {

                leftDiag += arr[i][i];
                rightDiag += arr[i][arr.Count - i - 1];

            }

            return Math.Abs(rightDiag - leftDiag);

        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {

            DiagonalSum sum = new();
            List<string> diag = new();
            List<List<int>> arr = new();

            if (sum.Id == System.Guid.Empty)
            {

                return BadRequest();

            }

            try
            {

                diag = input.TrimEnd().Split(',').ToList(); //converte lista em matriz, utilizando as virgulas
                diag.ForEach(x => arr.Add(x.TrimEnd().Split(' ').ToList().Select(dia => Convert.ToInt32(dia)).ToList())); // converte lista string para int

                sum.Input = input;
                sum.Output = Summary(arr).ToString();

                context.DiagonalSums.Add(sum);
                context.SaveChanges();

                return Ok(sum);

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

                return Ok(context.DiagonalSums.ToList().OrderByDescending(diag => diag.Date));

            }
            catch (Exception)
            {

                return NoContent();

            }

        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {

            try
            {
                var diagonalSum = context.DiagonalSums.FirstOrDefault(diag => diag.Id == id);

                if (diagonalSum == null)
                {

                    return NoContent();

                }
                else
                {

                    return Ok(diagonalSum);

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

                var diagonalSum = context.DiagonalSums.FirstOrDefault(diag => diag.Id == id);
                context.DiagonalSums.Remove(diagonalSum);
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

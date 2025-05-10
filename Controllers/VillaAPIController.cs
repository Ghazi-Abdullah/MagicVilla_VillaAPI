using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Controllers

//We need to replace the villa stor wiht the dbcontext
{
    //To retreve the logs we can install package "serilog."

    //We need rout the controller to the API
    //[Route("api/Controller")]
    [Route("api/VillaAPI")]
    //this attrebut which is the API conteroller is used to define the route of the controller
    [ApiController]
    //ControllerBase the controller base contains the common methods for returning all the data and users that is related to the controller's internet application.
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public VillaAPIController(ApplicationDbContext db) { 
        _db = db;
        }
        //private readonly ILogging _logger;
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}
        //To use the logger we need to use the ILogger" Dependency injiction " to create the login credintials
        //private readonly ILogger<VillaAPIController> _logger;
        //We need to use the constructor to create the instance of the logger

        /*public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }*/

        //We need use the attrebute to get the villas details [httpget]. mean to get the end point 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //we will use the ActionResult to return the data from the controller
        public ActionResult <IEnumerable<VillaDTO>> GetVillas()
        {
            //We need to log the message to the console
            //_logger.LogInformation("GetVillas was called");
            //_logger.Log("GetVillas was called", "");
            //We will use OK to return the status code 200

            //return Ok(VillaStore.villaList);
            return Ok(_db.Villas.ToList());

        }


        //This end point is used to get the villa details by id "only one record "
        [HttpGet("{id:int}", Name = "GetVilla")]
        //To returen the status for the response
                                    //We can select the type 
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]//success
        //[ProducesResponseType(404)]//not found
        //[ProducesResponseType(400)]//bad request

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult <VillaDTO> GetVilla(int id)
        {


            if(id == 0)
            {
                //This it will retrev the error on the console "cmd"
               // _logger.Log("GetVilla was called with id" + id, "error");

                return BadRequest();
            }
            //When we was use the villastor
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if(villa == null) 
            { 
            return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //We use the FromBody attribute to get the data from the body of the request
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO) 
        {
            //if(!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}

            //We will costomize the validation for the villaDTO            
            // This means that the villa is not unique
            //if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDOT.Name.ToLower()) != null)
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            { 
                ModelState.AddModelError("CustomError", "Villa name alraedy Exiats");
                return BadRequest(ModelState);
            }
            if (villaDTO == null) 
            {
                return BadRequest(villaDTO);
            }
            //If the id greater than 0 that means that this not a craete request  
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // We need to get the maximum id from the list of villas and increment the number for the new Id  i will use the "OrderByDesending"
            //villaDOT.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            //We need to add the new villa to the list of villas

            //We will convert the villaDTO to the villa manually
            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
            };
            _db.Villas.Add(model);
            _db.SaveChanges();


            //VillaStore.villaList.Add(villaDOT);
            //This means that the villa has been created successfully and we will add the new id to the route " id = villaDOT.Id "
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id},villaDTO);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //If we use the ActionResult we can define what's the return type
        //If we use the IActionResult we can not define what's the return type
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //VillaStore.villaList.Remove(villa);
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        //We need to update the villa by id
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //This means retreve the content from the body of the request as per the parameter"id"
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == villaDTO.Id);
            //if (villa == null)
            //{
            //    return NotFound();
            //}
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;

            //We will convert the villaDTO to the villa manually
            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();


            return NoContent();
        }

        //To see all the operation we can do using the patch we can viset the "jasonpatch.com"

        [HttpPatch("{id:int}", Name = "PartiallyUpdateVilla")]
        //This means that we will update the villa partially "one properites"
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PartiallyUpdateVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDoc)
        {
            if (patchDoc == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //We don't want to trak the villa we can useing the " AsNoTracking() "
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            //this is another way to update the villa
            //villa.Name = " new name ";
            //_db.SaveChanges();

            //We will convert the villa to the villaDTO manually
            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Amenity = villa.Amenity,
            };

            if (villa == null)
            {
                return BadRequest();
            }

            //We need to apply the patch to the villa to apply the update
            patchDoc.ApplyTo(villaDTO, ModelState);
            //We will convert the villaDTO to the villa manually
            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
            };

            //we want the entety framwork to track the model and changes
            _db.Villas.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
            //Json need to pass as an arraya 
        }

    }
}

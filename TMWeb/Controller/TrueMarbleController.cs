using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TMWeb.Controller
{
    public class TrueMarbleController : ApiController
    {


        /* Fuction: ViewImage
         * Type: Get
         * Parameters: zoom level, x coordinate, y coordinate
         * Return: byte[]
         * Assertion:  function returns the tile images.
         */

        [HttpPost]
        [Route("View")]
        public byte[] Post()
        {
            Dictionary<string, object> req = (Dictionary<string, object>)Request.Content.ReadAsAsync<Dictionary<string, 
                object>>().Result;

            int zoom = Convert.ToInt32(req["num1"]);
            int x = Convert.ToInt32(req["num2"]);
            int y = Convert.ToInt32(req["num3"]);


            try
            {
                int width, height, jpgSize, SizeStatus, finalstatus;
                SizeStatus = TrueMarbleLibrary.TrueMarble.GetTileSize(out width, out height);

                int buffSize = width * height * 3;
                byte[] buff = new byte[buffSize];

                finalstatus = TrueMarbleLibrary.TrueMarble.GetTileImageAsRawJPG(zoom, x, y, out buff, buffSize, out jpgSize);

                if (finalstatus == 1)
                {
                    return buff;
                }
                else
                {
                    return null;
                }
            }
            catch (System.DllNotFoundException ex)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));

            }
            catch (System.BadImageFormatException ex)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));
            }
            catch (System.InvalidCastException ex)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));
            }



        }


        /* Fuction: zoom
         * Type: Get
         * Parameters: NO PARAMETERS
         * Return: the number of zoom levels
         */
        [HttpGet]
        [Route("TMWeb/Zoom")]
        public int Get()
        {
            return 6;
        }


        /* Fuction: AcrossAndDown
         * Type: Get
         * Parameters: zoom level
         * Return: across:
         *         down:
         * function: will validate the zoom, and return the
         *           number of tiles across and down
         */
        [HttpGet]
        [Route("TMWeb/AcrossAndDown/{Zoom}")]
        public Dictionary<string, int> Get(int zoom)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();

            try
            {
                int x, y, status;
                status = TrueMarbleLibrary.TrueMarble.GetNumTiles(zoom, out x, out y);
         

                if (status == 0)
                {
                    Exception ex = null;
                    throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));
                }
                else
                {
                    ret.Add("across", x);
                    ret.Add("down", y);

                    return ret;
                }
            }
            catch (System.DllNotFoundException ex)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));
            }
            catch (System.BadImageFormatException ex)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(
HttpStatusCode.InternalServerError, "Error occurred : " +
ex.Message));
            }
        }
    }
}

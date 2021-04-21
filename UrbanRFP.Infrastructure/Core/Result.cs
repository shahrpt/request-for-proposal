namespace UrbanRFP.Infrastructure.Core
{
    public class Response<T>
    {
        #region Properties

        /// <summary>
        /// Success status returned from DB.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message returned from database
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Result Object Wrapper
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Status Code
        /// </summary>
        public Status Status { get; set; }

        #endregion

        #region Constructor

        public Response(T param)
        {
            Result = param;
        }

        #endregion
    }
}

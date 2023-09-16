namespace QueryBuilder
{
    /// <summary>
    /// Class to create HTML pages and handle QueryBuilder needs
    /// </summary>
    class PageCreator
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PageCreator()
        { }

        /// <summary>
        /// Create HTML page
        /// </summary>
        /// <returns>HTML page</returns>
        public string GetPage()
        {
            return
                "<!DOCTYPE>" +
                "<html>" +
                "  <head>" +
                "    <title>Query Builder</title>" +
                "  </head>" +
                "  <body>" +
                "    <form method=\"post\" action=\"shutdown\">" +
                "      <input type=\"submit\" value=\"Shutdown\" {0}>" +
                "    </form>" +
                "  </body>" +
                "</html>";
        }
    }
}

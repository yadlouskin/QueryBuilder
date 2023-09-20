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
        /// Create HTML page using jQuery QueryBuilder
        /// </summary>
        /// <param name="filters">Filters are created for data model</param>
        /// <returns>HTML page</returns>
        public string GetPage(string filters = "{}")
        {
            return
// TODO: It will be better to create separate HTML and JS files, but right now it is not critical
@"
<!DOCTYPE>
<html>
  <head>
    <title>Query Builder</title>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css' />
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/jQuery-QueryBuilder/dist/css/query-builder.default.min.css' />
  </head>
  <body>
    <h3>Query Builder</h3>
    <div id='builder'></div>
    <button class='btn btn-warning' id='btn-reset'>Reset</button>
    <button class='btn btn-primary' id='btn-get'>Get Rules</button>
    <button class='btn btn-primary' id='btn-get-mongo'>Get Query</button>
    <button class='btn btn-success' id='btn-get-data'>Get data from MongoDb</button>
    <br /><br />
    <div>
      <h3>Data from MongoDb for current query</h3>
      <p id='dataFromMongo'></p>
    </div>
    <script src='https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/jquery-extendext@1.0.0/jquery-extendext.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/moment@2.29.4/moment.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/jQuery-QueryBuilder/dist/js/query-builder.min.js'></script>
    <script>

setTimeout(() => {
  $('[data-toggle=""tooltip""]').tooltip();
}, 1000);

// const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle=""tooltip""]')
// const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

$('#builder').queryBuilder({
//  plugins: {'bt-tooltip-errors': { delay: 4000 }},
  
  filters: [" + filters + @"],

  rules: [{empty: true}]
});

$('#btn-reset').on('click', function() {
  $('#builder').queryBuilder('reset');
});

$('#btn-get').on('click', function() {
  var result = $('#builder').queryBuilder('getRules');
  
  if (!$.isEmptyObject(result)) {
    alert(JSON.stringify(result, null, 2));
  }
});

$('#btn-get-mongo').on('click', function() {
  var result = $('#builder').queryBuilder('getMongo');

  if (!$.isEmptyObject(result)) {
    alert(JSON.stringify(result, null, 2));
  }
});

$('#btn-get-data').on('click', function() {

});

$('body').click(() => {
  $('.pull-right').addClass('float-end');
});
$('.pull-right').addClass('float-end');

    </script>
  </body>
</html>
";
        }
    }
}
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
    <button class='btn btn-success' id='btn-get-data'>Get data from MongoDb</button>
    <br /><br />
    <div>
      <h3>Data from MongoDb for current query:</h3>
      <p id='dataFromMongo'></p>
      <br />
      <table>
        <tr>
          <th style='padding:0px 20px;'><h4>Query:</h4></th>
          <th style='padding:0px 20px;'><h4>Rules:</h4></th>
        </tr>
        <tr>
          <td id='td_query' style='padding:0px 20px;vertical-align:top;'></td>
          <td id='td_rules' style='padding:0px 20px;vertical-align:top;'></td>
        </tr>
      </table>
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

$('#btn-get-data').on('click', function() {
  var rules = $('#builder').queryBuilder('getRules');
  var query = $('#builder').queryBuilder('getMongo');

  if (!$.isEmptyObject(query) && !$.isEmptyObject(rules)) {
    var beautify = (s) =>
      JSON.stringify(s, null, '&nbsp;')
      .replaceAll('\n', '<br />')
      .replaceAll('&nbsp;', '&nbsp;&nbsp;&nbsp;&nbsp;');
    $('#td_query').empty().append(beautify(query));
    $('#td_rules').empty().append(beautify(rules));

    var requestedData = $.post('/getdata', JSON.stringify(query));
    requestedData.done( (data) => {
      $('#dataFromMongo').empty().append(data);
    });
  }
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
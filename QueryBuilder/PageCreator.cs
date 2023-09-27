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

function changeFilter(rule_id){
  $('select[name=' + rule_id + '_filter]').val(-1).change();
  var compType = $('select[name=' + rule_id + '_comptype]').find(':selected').val();

  $('select[name=' + rule_id + '_filter]').children().each(function() {
    var attrHidden = $(this).attr('hidden');
    var optionValue = $(this).attr('value');

    var hasHidden = typeof attrHidden !== 'undefined' && attrHidden !== false;
    var needRemove = hasHidden && optionValue != -1 && optionValue.endsWith(compType);
    var needAdd = !hasHidden && optionValue != -1 && !optionValue.endsWith(compType);

    if (needRemove){
      $(this).removeAttr('hidden');
    }
    if (needAdd){
      $(this).attr('hidden', true);
    }
  });
}


$('#builder').queryBuilder({
//  plugins: {'bt-tooltip-errors': { delay: 4000 }},
  
  filters: [" + filters + @"],

  rules: [{empty: true}],

  templates: {
    filterSelect: function(qb) {
      let optgroup = null;

      return `
<!--<label for=""${qb.rule.id}_comptype"" class=""form-label"">Compare property with</label>-->
<select name=""${qb.rule.id}_comptype"" class=""form-select"" onchange=""changeFilter('${qb.rule.id}');"">
  <option value=""-1"">Choose comparison type</option>
  <option value=""_val"">Compare with value</option>
  <option value=""_opt"">With other properties</option>
  <option value=""_dat"">With date values</option>
  <!--<option value=""_exp"">Using expression</option>-->
</select>

<select class=""form-control"" name=""${qb.rule.id}_filter"">
  ${qb.settings.display_empty_filter ? `
    <option value=""-1"">Choose property</option>
  ` : ''}
  ${qb.filters.map(filter => `
    ${optgroup !== filter.optgroup ? `
      ${optgroup !== null ? `</optgroup>` : ''}
      ${(optgroup = filter.optgroup) !== null ? `
        <optgroup label=""${qb.translate(qb.settings.optgroups[optgroup])}"">
      ` : ''}
    ` : ''}
    <option value=""${filter.id}"" ${filter.icon ? `data-icon=""${filter.icon}""` : ''} hidden>${qb.translate(filter.label)}</option>
  `).join('')}
  ${optgroup !== null ? '</optgroup>' : ''}
</select>`;
    }
  },
});

$('#btn-reset').on('click', function() {
  $('#builder').queryBuilder('reset');
});

$('#btn-get-data').on('click', function() {
  var rules = $('#builder').queryBuilder('getRules');
  var query = $('#builder').queryBuilder('getMongo');

  if (!$.isEmptyObject(query) && !$.isEmptyObject(rules)) {
    query = JSON.stringify(query)
    while (optDate = query.match(/_optDateToUseExpr(""\:\{""\$[a-z]+""\:)""(\d+)\/(\d+)\/(\d+)""/)) {
      newDate = optDate[1] +
        (parseInt(optDate[4]) * 10000 + parseInt(optDate[3]) * 100 + parseInt(optDate[2]));
      query = query.replace(optDate[0], newDate);
    }
    while (optDate = query.match(/_optDateToUseExpr(""\:)""(\d+)\/(\d+)\/(\d+)""/)) {
      newDate = optDate[1] +
        (parseInt(optDate[4]) * 10000 + parseInt(optDate[3]) * 100 + parseInt(optDate[2]));
      query = query.replace(optDate[0], newDate);
    }

    query = query.replaceAll(
      /""([\w\.]+)_optFirstToUseExpr""\:""([\w\.]+)_optSecondToUseExpr""/g,
      '""\$expr""\:\{""\$eq""\:\[""$$$1"",""$$$2""\]\}'
    );
    query = query.replaceAll(
      /""([\w\.]+)_optFirstToUseExpr""\:\{""\$([a-z]+)""\:""([\w\.]+)_optSecondToUseExpr""\}/g,
      '""\$expr""\:\{""$$$2""\:\[""$$$1"",""$$$3""\]\}'
    );

    var beautify = (s) =>
      JSON.stringify(s, null, '&nbsp;')
      .replaceAll('\n', '<br />')
      .replaceAll('&nbsp;', '&nbsp;&nbsp;&nbsp;&nbsp;');
    $('#td_query').empty().append(beautify(JSON.parse(query)));
    $('#td_rules').empty().append(beautify(rules));

    var requestedData = $.post('/getdata', query);
    requestedData.done( (data) => {
      $('#dataFromMongo').empty().append(data);
    });
  }
});

function fixStyle() {
  $('.pull-right').addClass('float-end').removeClass('pull-right');
  $('select.form-control').addClass('form-select').removeClass('form-control');
  $('.form-select').css('display', 'inline-block');
  $('.form-select').css('width', 'auto');
  $('.form-select').css('padding', '.375rem 2.25rem .375rem .75rem');
}

$('body').click(() => {
  fixStyle()
});
fixStyle()

    </script>
  </body>
</html>
";
        }
    }
}
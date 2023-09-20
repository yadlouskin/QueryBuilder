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
    <div id='builder'></div>
    <button class='btn btn-success' id='btn-set'>Set Rules</button>
    <button class='btn btn-primary' id='btn-get'>Get Rules</button>
    <button class='btn btn-warning' id='btn-reset'>Reset</button>

    <form method='post' action='shutdown'>
      <input type='submit' value='Shutdown'>
    </form>
    <script src='https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/jquery-extendext@1.0.0/jquery-extendext.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/moment@2.29.4/moment.min.js'></script>
    <script src='https://cdn.jsdelivr.net/npm/jQuery-QueryBuilder/dist/js/query-builder.min.js'></script>
    <script>

var rules = {
  condition: 'AND',
  rules: [{
    id: 'price',
    operator: 'less',
    value: 10.25
  }, {
    condition: 'OR',
    rules: [{
      id: 'category',
      operator: 'equal',
      value: 2
    }, {
      id: 'category',
      operator: 'equal',
      value: 1
    }]
  }]
};

setTimeout(() => {
  $('[data-toggle=""tooltip""]').tooltip();
}, 1000);

// const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle=""tooltip""]')
// const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

$('#builder').queryBuilder({
//  plugins: {'bt-tooltip-errors': { delay: 4000 }},
  
  filters: [{
    id: 'name',
    label: 'Name',
    type: 'string'
  }, {
    id: 'category',
    label: 'Category',
    type: 'integer',
    input: 'select',
    values: {
      1: 'Books',
      2: 'Movies',
      3: 'Music',
      4: 'Tools',
      5: 'Goodies',
      6: 'Clothes'
    },
    operators: ['equal', 'not_equal', 'in', 'not_in', 'is_null', 'is_not_null']
  }, {
    id: 'in_stock',
    label: 'In stock',
    type: 'integer',
    input: 'radio',
    values: {
      1: 'Yes',
      0: 'No'
    },
    operators: ['equal']
  }, {
    id: 'price',
    label: 'Price',
    type: 'double',
    validation: {
      min: 0,
      step: 0.01
    }
  }, {
    id: 'id',
    label: 'Identifier',
    type: 'string',
    placeholder: '____-____-____',
    operators: ['equal', 'not_equal'],
    validation: {
      format: /^.{4}-.{4}-.{4}$/
    }
  }],

  rules: rules
});

$('#btn-reset').on('click', function() {
  $('#builder').queryBuilder('reset');
});

$('#btn-set').on('click', function() {
  $('#builder').queryBuilder('setRules', rules);
});

$('#btn-get').on('click', function() {
  var result = $('#builder').queryBuilder('getRules');
  
  if (!$.isEmptyObject(result)) {
    alert(JSON.stringify(result, null, 2));
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
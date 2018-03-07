using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LspAnalyzer.Analyze
{
    public class AggregateGridFilter
    {
        readonly List<string> _columnName;
        readonly List<TextBox> _control;
        readonly BindingSource _bs;

        /// <summary>
        /// Constructor to aggregate filters.
        /// Filters are defined in 'DataColumn.Expression Property'
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="columnName">Column name to filter</param>
        /// <param name="control">The control,currently only TextBox</param>
        public AggregateGridFilter(BindingSource bs, List<string> columnName, List<TextBox> control)
        {
            _bs = bs;
            _columnName = columnName;
            _control = control;
        }

        public void FilterReset()
        {
            _bs.Filter = null;
        }
        /// <summary>
        /// Filter the form
        /// </summary>
        public void FilterGrid(bool startAtBeginning = false)
        {
            string firstWildCard = "";
            if (startAtBeginning) firstWildCard = "%";
            // Filters to later aggregate to string
            var lFilters = new List<string>();
            int i = 0;
            foreach (var f in _columnName)
            {
                GuiHelper.AddSubFilter(lFilters, firstWildCard, f, _control[i].Text);
                i += 1;
            }
     

            string filter = GuiHelper.AggregateFilter(lFilters);

            // enter filter and check for exceptions
            try
            {
                _bs.Filter = filter;
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Filter: '{filter}'
The Filter may contain:
- '%' or '*' wildcard at string start
- 'NOT ' preceding the filter string

Examples:
'myFilter'
'%myFilter'
'NOT myFilter'
'NOT %myFilter'

hoReverse always adds a wildcard at string end! hoReverse combines filters by ' AND '

Not allowed are wildcard '*' or '%' amidst the filter string.


{e}",

                    "The filter you have defined is invalid!");
                _bs.Filter = "";
            }



        }
    }
}

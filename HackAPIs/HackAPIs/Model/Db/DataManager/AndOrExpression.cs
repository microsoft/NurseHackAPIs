using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class AndOrExpression //: ExpressionBase<FilmTimeEntity>
    {
       /*
            public override IQueryable<FilmTimeEntity> GetExampleQuery(LinqExpressionsDbContext context)
            {
                var expression = GetExpression(context);

                // Store the filter as a dynamic query.
                return context.FilmTimeEntities.Where(expression);
            }

            protected override Expression<Func<FilmTimeEntity, bool>> GetExpression(LinqExpressionsDbContext context)
            {
                var beginTime = DateTimeOffset.Parse("2019-08-03 12:00");
                var endTime = DateTimeOffset.Parse("2019-08-04 11:00");
                // We want to build the expression for the "Where" clause

                // Example of Query we will eventually run.
                // context.FilmTimeEntities.Where(ft => ft.FilmId == 3 &amp;&amp; (ft.StartTime < beginTime || ft.StartTime >= endTime));

                // Time to build up the clause in the ANY field
                var ftParameter = Expression.Parameter(typeof(FilmTimeEntity), "ft"); // ft =>

                var ftIdProperty = Expression.Property(ftParameter, "FilmId"); // ft.FilmId
                var ftIdClause = Expression.Equal(ftIdProperty, Expression.Constant(3)); // ft.FilmId == 3

                // Begin the OrElse statement
                var ftStartTimeProperty = Expression.Property(ftParameter, "StartTime"); // ft.StartTime
                var ftStartTimeFirstClause = Expression.LessThan(ftStartTimeProperty, Expression.Constant(beginTime)); // ft.StartTime < beginTime
                var ftStartTimeSecondClause = Expression.GreaterThanOrEqual(ftStartTimeProperty, Expression.Constant(endTime)); // ft.StartTime >= endTime

                // Or statement
                var ftOrElseClause = Expression.OrElse(ftStartTimeFirstClause, ftStartTimeSecondClause); // (ft.StartTime < beginTime || ft.StartTime >= endTime)

                // AndAlso statement 
                var ftAndClause = Expression.AndAlso(ftIdClause, ftOrElseClause); // ft.FilmId == 3 &amp;&amp; (ft.StartTime < beginTime || ft.StartTime >= endTime)

                // Lambda Expression
                return Expression.Lambda<Func<FilmTimeEntity, bool>>(ftAndClause, ftParameter);

            }


        }*/
    }
}

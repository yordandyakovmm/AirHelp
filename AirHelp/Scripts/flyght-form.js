function progress(_this)
{
    var res = validateFlight();
    if (res) {
        var flights = [];
        var first = $('[name="DepartureAirport"]').data('data');
        var last = $('[name="DestinationAirports"]').data('data');
        var airports = [];

        $('[name=ConnectionAirports]:visible').each(function (index) {
            var data = $($('[name=ConnectionAirports]')[index]).data('data');
            airports.push(data);
        });

        if (airports.length == 0) {
            var flight = {
                number: first.iata,
                departure: first.country + ' (' + first.city + ') ',
                arrival: last.country + ' (' + last.city + ') '
            };

            flights.push(flight);
        }

        if (airports.length > 0) {

            var flight = {
                number: first.iata,
                departure: first.country + ' (' + first.city + ') ',
                arrival: airports[0].country + ' (' + airports[0].city + ') '
            };
            flights.push(flight);

            for (var i = 0; i < airports.length - 1; i++) {
                var flight = {
                    number: airports[i].iata,
                    departure: airports[i].country + ' (' + airports[i].city + ') ',
                    arrival: airports[i + 1].country + ' (' + airports[i + 1].city + ') '
                };
                flights.push(flight);
            }

            var flight = {
                number: airports[airports.length - 1].country.iata,
                departure: airports[airports.length - 1].country + ' (' + airports[airports.length - 1].city + ') ',
                arrival: last.country + ' (' + last.city + ') '
            };
            flights.push(flight);

        }

        window.flights = flights;

        if (flights.length > 1) {
            $('[choise-flight].form-row-radio').show();

            for (var i = 0; i < flights.length; i++) {
                var tempate = $('#template1').html();
                tempate = tempate
                    .replace('{1}', flights[i].number)
                    .replace('{2}', flights[i].departure + ' -- ' + flights[i].arrival);
                $('[choise-flight].form-row-radio').append(tempate);

                tempate = $('#template2').html();
                tempate = tempate
                    .replace('{1}', flights[i].number)
                    .replace('{2}', flights[i].departure + ' -- ' + flights[i].arrival);
                $('[multinumber]').append(tempate);
            }
            $('[choise-flight]').show();

        }
        else {
            $('[post]').show();
            $('[number]').show();
            
        }
        
        $('[first] input').attr('disabled', 'disabled');
        $('[first]').addClass('blur');

        $('[progress]').hide();
        
    }
}


function validateFlight() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });
    return result;
}


function flightChange(_this)
{
    $(_this).parent().parent().find('label').removeClass('selected');
    $(_this).parent().addClass('selected');
    $('[multinumber-row]').show();
    $('[multinumber]').show();
    $('[post]').show();
    $('[first] input').attr('disabled', 'disabled');
    $('[first]').addClass('blur');

}
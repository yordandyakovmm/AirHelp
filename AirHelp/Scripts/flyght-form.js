function progress(_this)
{
    var res = validateCommon();
    if (res) {
        var flights = [];
        var first = $('[name="DepartureAirport"]').data('data');
        var last = $('[name="DestinationAirports"]').data('data');
        var airports = $('[name="connectionAirPorts"]');

        if (airports.length == 0) {
            debugger;
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

        if (flights.length > 1) {
            $('[choise-flight].form-row-radio').show();

            for (var i = 0; i < airports.length - 1; i++) {
                var tempate = $('#template1').html();
                tempate = tempate
                    .replace('{1}', airports[i].number)
                    .replace('{2}', airports[i].departure + ' -- ' + airports[i].arrival);
                $('[choise-flight].form-row-radio').append(tempate);
            }
        }
        else {
            $('[number]').show();
        }
        
        $('[pragress]').hide();
        
    }
}
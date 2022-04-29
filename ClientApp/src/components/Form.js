import axios from 'axios';
import TimeField from 'react-simple-timefield';
import React, { Component } from 'react';

export class Form extends React.Component {
    constructor(props) {
        super(props);
        this.state = { value1: 1, value2: 1, typeRoute: "fastest", isRouteCalculate: false, route: { buses:[], stops: [], totalTime: 0, totalCost:0 }, time: '00:00'};

        this.handleChange1 = this.handleChange1.bind(this);
        this.handleChange2 = this.handleChange2.bind(this);
        this.handleChange3 = this.handleChange3.bind(this);
        this.onTimeChange = this.onTimeChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange1(event) {
        this.setState({ value1: event.target.value });
    }

    handleChange2(event) {
        this.setState({ value2: event.target.value });
    }
    handleChange3(event) {
        this.setState({ typeRoute: event.target.value });
    }
    onTimeChange(event, time) {
        this.setState({ time });
    }
    handleSubmit(event) {
        axios.get("https://localhost:7056/api/route", { params: { src: this.state.value1, dst: this.state.value2, type: this.state.typeRoute, hrs: this.state.time.split(':')[0], min: this.state.time.split(':')[1]} })
            .then(res => {
                const route = res.data;
                this.setState({ route });
                this.state.isRouteCalculate = true;
                console.log(route);
            });
        event.preventDefault();
    }

    static renderTable(route) {
        let data = route.totalCost > 0
            ? <p>{route.totalCost} руб.</p>
            : <p> { route.totalTime } мин. </p>
    
        return (
            <div>
                <h3>Наилучший маршрут: {data}</h3>
                    <p>Остановки</p>
                    {route.stops.map((item,index) =>(
                        <li key={index}>{item}</li>
                    ))}
                <p>Соответствующие автобусы </p>
{route.buses.map((item, index) => (
<li key={index}>{item}</li>
))}

                    </div>
    );
}

    render() {
        let contents = !this.state.isRouteCalculate
            ? <p></p>
        : Form.renderTable(this.state.route);
        return (
            <div>
        <form onSubmit={this.handleSubmit}>
            <label>
            Выберите начальную остановку:
                    <select value={this.state.value1} onChange={this.handleChange1}>
                        {this.props.stops.map(item => (
                <option key={item} value={item}>{item}</option>
        ))}

            </select>
                </label>
                <div>  </div>
            <label>
            Выберите конечную остановку:
        <select value={this.state.value2} onChange={this.handleChange2}>
{this.props.stops.map(item => (
    <option key={item} value={item}>{item}</option>
))}
            </select>
                </label>
                <div>  </div>
    <label>
    Выберите тип маршрута:
        <select value={this.state.typeRoute} onChange={this.handleChange3}>
            
    <option value="fastest">Самый быстрый</option>
    <option value="cheapest">Самый дешёвый</option> 

    </select>
                </label>
                <div>  </div>
    <label>
                    Выберите время:

    <TimeField value={this.state.time} style={{width: 50}} onChange={this.onTimeChange} />
                </label>
    <div>  </div>
            <input type="submit" value="Рассчитать" />
            </form>
                {contents}
</div>
);
}
}

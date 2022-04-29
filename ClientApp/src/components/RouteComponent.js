import axios from 'axios';
import React, { Component } from 'react';
import { Upload } from './Upload';
import { Form } from './Form';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { route: [], isFileUpload: false, stops: [] };
  }

  updateData = (value) => {
      this.setState({ isFileUpload: value });
      this.populateStopsData()
  }


    static renderTable(stops) {
        return (
            <div>
                <Form stops = {stops}/>
            </div>
    );
  }

  render() {
    let contents = !this.state.isFileUpload
        ? <p><em> </em></p>
      : FetchData.renderTable(this.state.stops);

      return (
                <div>
            <h1 id="tabelLabel" >Routes</h1> 
        <Upload updateData = {this.updateData}/>
            {contents}
        </div>

    );
  }

  async populateStopsData() {

      axios.get("https://localhost:7056/api/stops")
          .then(res => {
              const stops = res.data;
              this.setState({ stops });
              console.log(stops);
          });

  }
}

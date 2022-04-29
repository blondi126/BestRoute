import React, { Component } from 'react';
import { FetchData } from './RouteComponent';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <FetchData />
      </div>
    );
  }
}

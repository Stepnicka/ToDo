import React, { Component } from 'react';
import { Routes, Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ToDo } from './components/ToDo';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Routes>
          <Route exact path='/' element={<Home/>} />
          <Route path='/toDo' element={<ToDo/>} />
        </Routes>
      </Layout>
    );
  }
}

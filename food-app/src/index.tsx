import React from 'react';
import ReactDOM from 'react-dom';
import { App } from './App';
import { createBrowserHistory } from 'history';
import { Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './store';
import './i18n';

const history = createBrowserHistory();

ReactDOM.render(
    <React.StrictMode>
        <Provider store={store}>
            <Router history={history}>
                <App />
            </Router>
        </Provider>
    </React.StrictMode>,
    document.getElementById('root'),
);

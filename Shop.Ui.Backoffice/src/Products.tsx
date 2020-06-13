import React, { Component } from 'react';
import * as Oidc from 'oidc-client';

// declare var Oidc: any;
var config = {
    authority: "https://localhost:1001",
    client_id: "Shop.Ui.Backoffice",
    redirect_uri: "http://localhost:3002/callback",
    response_type: "code",
    scope: "openid profile Shop.Api.Products",
    post_logout_redirect_uri: "http://localhost:3002/",
    filterProtocolClaims: true,
    loadUserInfo: true
};
var mgr = new Oidc.UserManager(config);

class Products extends Component {
    state = {
        user: {},
        products: [],
    };

    componentDidMount() {
        mgr.getUser().then(user => {
            this.setState({ user });
            console.log(this.state);
        });

        mgr.signinRedirectCallback().then(user => {
            console.log(user);
            this.setState({ user });
        }).catch(err => { });
    }

    listProducts = () => {
        const user: any = this.state.user;
        const products: any[] = this.state.products;

        var myHeaders = new Headers({
            'Authorization': `${user?.token_type} ${user?.access_token}`,
        });

        var options: any = {
            method: 'GET',
            headers: myHeaders,
            mode: 'cors',
            cache: 'default'
        };

        fetch('https://localhost:2001/products', options)
            .then(response => response.json())
            .then(data => {
                this.setState({ products: data });
            })
            .catch(() => console.log('Errouuuuu'));
    }

    login() {
        mgr.signinRedirect();
    }

    logout() {
        mgr.signoutRedirect();
    }

    getUserProfile() {
        mgr.getUser().then(function (user: any) {
            if (user) {
                console.log("User logged in", user.profile);
            }
            else {
                console.log("User not logged in");
            }
        });
    }

    render() {
        const user: any = this.state.user;
        const products: any[] = this.state.products;

        var userInfo;
        if (this.state.user) {
            userInfo = <p>
                <small>
                    <strong>Token</strong>: {user?.access_token}<br />
                    <strong>Header</strong>: {user?.token_type} {user.access_token}
                </small>
            </p>;
        }

        return (<div>
            <h1>Produtos</h1>

            {userInfo}

            <button onClick={this.login}>Login</button>
            <button onClick={this.logout}>Logout</button>
            <button onClick={this.getUserProfile}>User Profile</button>
            <button onClick={this.listProducts}>Listar</button>

            <ul>
                {products?.map((prod: any) =>
                    <li key={prod.id}>
                        {prod.name}
                    </li>
                )}
            </ul>
        </div>);
    }
}

export default Products;
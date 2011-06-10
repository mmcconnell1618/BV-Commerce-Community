$(document).ready(function () {

    var header = "";
    header += '<div id="header">';
    header += '<div id="logo-area"><a href="Home.html">Stone <span class="logo2">&amp; Slate</span></a></div>';
    header += '<div id="cart-link"><a href="Cart.html"><span>View Cart: 0 items</span></a></div>';
    header += '<div id="links-area"><ul><li><a class="myaccountlink" href="MyAccount.html"><span>My Account</span></a></li>';
    header += '<li><a class="signinlink" href="SignIn.html"><span>Sign In</span></a></li>';
    header += '<li><a class="contactlink" href="Checkout.html"><span>Checkout</span></a></li></ul></div>';
    header += '<div id="search-form"><div class="searchform"><input type="textbox" id="headersearchbox"> <a href="#" id="headersearchlink"><span>Search</span></a></div></div>';
    header += '<div id="header-menu">';
    header += '<ul>';
    header += '<li class="activemainmenuitem"><a href="Category.html" tabindex="0" class="actuator" title="Sample Category of Products"><span>Sample Products</span></a></li>';
    header += '<li><a href="Category.html" tabindex="1" class="actuator" title="More Sample Products"><span>More Sample</span></a></li>';
    header += '<li><a href="Category.html" tabindex="2" class="actuator" title=""><span>Demo Category</span></a></li>';
    header += '<li><a href="CustomPage.html" tabindex="3" class="actuator" title=""><span>About Us</span></a></li></ul>';
    header += '</div>';
    header += '</div>';

    $('#dynamicheader').replaceWith(header);
});
function initSigniture() {

    //el = document.getElementById('signiture-div');
    //if (!el) {
    //    return;
    //}

    conv = document.getElementById('signiture');
    ctx = conv.getContext('2d');
    ctx.strokeStyle = 0;
    ctx.lineWidth = 3;
    ctx.lineJoin = 'round';
    ctx.strokeStyle = '#000000';
    ctx.shadowColor = '#000000';
    ctx.shadowBlur = 0.7;
    isDrawing = false;
    couunt = 0;


    //conv.ontouchstart = onmousedown;
    conv.addEventListener("touchmove", onmousemove1, false); 
    conv.addEventListener("touchend", onmouseup, false);





    conv.onmousedown = onmousedown;
    conv.onmouseup = onmouseup;
    conv.onmousemove = onmousemove;

    conv.mouseout = function (e) {
        x = y = null;
        isDrawing = false;

    };

    conv.mousein = function (e) {
        debugger;
        //console.log('in');
    };



}


function onmousedown(e) {
    e.preventDefault();
    isDrawing = true;
    x = e.offsetX ;
    y = e.offsetY ;
    ctx.moveTo(x, y);
    console.log(x, y);
    //e.stopPropagation();
};


function onmousemove(e) {
    e.preventDefault();
    if (isDrawing) {
        console.log(x, y);
        couunt++;
        var newX = e.offsetX; 
        var newY = e.offsetY;
        //var distance = (x - newX) ** 2 + (y - newY) ** 2;
        //setContext(ctx, distance, x, y);
        ctx.moveTo(x, y);
        ctx.lineTo(newX, newY);
        x = newX;
        y = newY;
        ctx.stroke();
    }
    if (couunt > 80) {
        $('.form-box-signiture').removeClass('error').addClass('success');
        //saveSigiture();
    }
};


function onmousemove1(e) {
    e.preventDefault();
    if (x) {
        console.log(x, y);
        couunt++;
        var rect = this.getBoundingClientRect();
        var newX = e.touches[0].clientX - rect.left;
        var newY = e.touches[0].clientY - rect.top;
        //var distance = (x - newX) ** 2 + (y - newY) ** 2;
        //setContext(ctx, distance, x, y);
        ctx.moveTo(x, y);
        ctx.lineTo(newX, newY);
        x = newX;
        y = newY;
        ctx.stroke();
    }
    else {
        var rect = this.getBoundingClientRect();
        x = e.touches[0].clientX - rect.left;
        y = e.touches[0].clientY - rect.top;
    }
    if (couunt > 80) {
        $('.form-box-signiture').removeClass('error').addClass('success');
        //saveSigiture();
    }
};

function onmouseup(e) {
    x = y = null
    isDrawing = false;
    saveSigiture();
    e.stopPropagation();

};



function saveSigiture() {
    var dataURL = conv.toDataURL('image/png');
    $('[name=SignitureImage]').val(dataURL);
}

function clearSignature() {
    conv.width = conv.width;
    $('.form-box-signiture').removeClass('success').removeClass('error');
    //var html = $('.form-box-signiture > div.convas-holder').html();
    //$('.form-box-signiture > div.convas-holder').html('');
    //$('.form-box-signiture > div.convas-holder').html(html);
    initSigniture();

}


function setContext(ctx, distance, x, y) {
    if (distance < 50 && type != 0) {
        type = 0;
        ctx.beginPath();
        ctx.lineWidth = 3;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 10;
        ctx.moveTo(x, y);
    }
    else if (distance < 200 && type != 1) {
        type = 1;
        ctx.beginPath();
        ctx.lineWidth = 2.8;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 5;
        ctx.moveTo(x, y);
    }
    else if (distance < 300 && type != 2) {
        type = 2;
        ctx.beginPath();
        ctx.lineWidth = 2.4;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 1.5;
        ctx.moveTo(x, y);
    }
    else if (distance < 400 && type != 3) {
        type = 3;
        ctx.beginPath();
        ctx.lineWidth = 2.2;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 0.5;
        ctx.moveTo(x, y);
    }
    else if (distance < 7000 && type != 4) {
        type = 4;
        ctx.beginPath();
        ctx.lineWidth = 2;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 0.3;
        ctx.moveTo(x, y);
    }
    else {
        type = 5;
        ctx.beginPath();
        ctx.lineWidth = 2;
        ctx.lineJoin = 'round';
        ctx.shadowBlur = 0.1;
        ctx.moveTo(x, y);
    }
}


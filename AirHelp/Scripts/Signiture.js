


function setContext(ctx, distance, x, y)
{
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


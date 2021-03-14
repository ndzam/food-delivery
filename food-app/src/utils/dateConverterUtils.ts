export function getDate(epochDate: number): Date {
    var d = new Date(0);
    d.setUTCSeconds(epochDate);
    return d;
}

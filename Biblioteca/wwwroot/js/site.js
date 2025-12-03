function cambiarTema() {
    const html = document.documentElement;
    const temaActual = html.getAttribute("data-tema");
    const nuevoTema = temaActual === "oscuro" ? "claro" : "oscuro";
    html.setAttribute("data-tema", nuevoTema);
    localStorage.setItem("temaPreferido", nuevoTema);
}

// Aplicar el tema guardado al cargar
document.addEventListener("DOMContentLoaded", () => {
    const temaGuardado = localStorage.getItem("temaPreferido") || "claro";
    document.documentElement.setAttribute("data-tema", temaGuardado);
});
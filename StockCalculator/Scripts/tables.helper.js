function createCell(tagName) {
	return document.createElement(tagName);
}

function createElementCell(element, tagName) {
	var td = createCell(tagName);
	td.appendChild(element);
	return td;
}

function createTextCell(text, tagName) {
	return createElementCell(document.createTextNode(text), tagName);
}

function createStocksRow(item, actionLinkInnerHtml) {
	var tr = document.createElement("tr");
	tr.appendChild(createTextCell(item.Name, "td"));
	tr.appendChild(createTextCell(item.Price, "td"));
	tr.appendChild(createTextCell(item.Quantity, "td"));
	tr.appendChild(createTextCell(item.Percentage, "td"));
	tr.appendChild(createTextCell(item.Years, "td"));
	var cell = createCell("td");
	cell.innerHTML = actionLinkInnerHtml;
	tr.appendChild(cell);
	return tr;
}

function createStocksHeader() {
	var tr = document.createElement("tr");
	tr.appendChild(createTextCell("Name", "th"));
	tr.appendChild(createTextCell("Price", "th"));
	tr.appendChild(createTextCell("Quantity", "th"));
	tr.appendChild(createTextCell("Percentage", "th"));
	tr.appendChild(createTextCell("Years", "th"));
	tr.appendChild(createTextCell("", "th"));
	return tr;
}

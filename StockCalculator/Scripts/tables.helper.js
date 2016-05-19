// -------------------------------------------------------------
// common methods
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

// -------------------------------------------------------------
// for stocks list
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

// -------------------------------------------------------------
// for one stock and stock result

function createFieldValueRow(field, value) {
	var tr = document.createElement("tr");
	tr.appendChild(createTextCell(field, "td"));
	tr.appendChild(createTextCell(value, "td"));
	return tr;
}

function createStockTable(tableElement, item) {
	var tr = document.createElement("tr");
	tr.appendChild(createTextCell("Field", "th"));
	tr.appendChild(createTextCell("Value", "th"));
	tableElement.appendChild(tr);
	tableElement.appendChild(createFieldValueRow("Stock name", item.Name));
	tableElement.appendChild(createFieldValueRow("Price", item.Price));
	tableElement.appendChild(createFieldValueRow("Quantity", item.Quantity));
	tableElement.appendChild(createFieldValueRow("Percentage", item.Percentage));
	tableElement.appendChild(createFieldValueRow("Years", item.Years));
}

function createResultTable(tableElement, values) {
	var tr = document.createElement("tr");
	tr.appendChild(createTextCell("Year", "th"));
	tr.appendChild(createTextCell("Value", "th"));
	tableElement.appendChild(tr);

	for(var value in values) {
		tableElement.appendChild(createFieldValueRow(value.Year, value.Value));
	}
}

Vue.component('split-resize', {
	template: '<div :class="classes" @mousedown="mouseDownEvent"></div>',
	data: function () {
		return {
			isDragging: false,
			lastWidthPx: 0.0,
			lastMousePosPx: 0.0,
			fontSize: 0.0
		};
	},
	props: ['classes', 'variable', 'dragComplete', 'initialValue'],
	watch: {
		initialValue: function (value) {
			this.setSizeInRem(value);
		}
	},
	methods: {
		setSizeInRem(newSize) {
			document.querySelector(':root').style.setProperty(this.variable, newSize + 'rem');
		},
		getFontSize() {
			return parseFloat(getComputedStyle(document.documentElement).fontSize);
		},
		getComputedRem(newPos) {
			var resizedValueRem = (newPos + this.lastWidthPx) / this.fontSize;
			if (resizedValueRem < 10.0) {
				resizedValueRem = 10.0;
			}
			return (resizedValueRem);
		},
		resize(newPos) {
			this.setSizeInRem(this.getComputedRem(newPos));
		},
		mouseDownEvent(event) {
			var rootStyle = getComputedStyle(document.documentElement);

			this.fontSize = this.getFontSize();
			this.lastWidthPx = parseFloat(rootStyle.getPropertyValue(this.variable)) / this.fontSize;
			this.lastMousePosPx = event.clientX;
			this.isDragging = true;

			event.preventDefault();

			document.addEventListener('mousemove', this.mouseMoveEvent);
			document.addEventListener('mouseup', this.mouseUpEvent);
		},
		mouseUpEvent(event) {
			if (this.isDragging) {
				document.removeEventListener('mousemove', this.mouseMoveEvent);
				document.removeEventListener('mouseup', this.mouseUpEvent);

				this.isDragging = false;
				this.resize(event.clientX);

				this.dragComplete(this.getComputedRem(event.clientX))
			}
		},
		mouseMoveEvent(event) {
			if (this.isDragging) {
				this.resize(event.clientX);

				event.preventDefault();
			}
		}
	}
});
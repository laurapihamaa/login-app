FROM node:16

# Set working directory for all build stages.
WORKDIR /app

# Copy package.json so that package manager commands can be used.
COPY package.json package-lock.json ./

RUN npm install

# Copy the rest of the source files into the image.
COPY . ./

# Run the build script.
RUN npm run build

# Expose the port that the application listens on.
EXPOSE 3000

# Run the application.
CMD ["npm", "start"]
